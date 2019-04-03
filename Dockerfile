FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app


#
# Build the project
#
FROM microsoft/dotnet:2.2-sdk AS build

WORKDIR /src
COPY ApacheIgniteNetKubernetesExample.sln ./
COPY ApacheIgniteNetKubernetesExample/ApacheIgniteNetKubernetesExample.csproj ./ApacheIgniteNetKubernetesExample/

RUN dotnet restore ApacheIgniteNetKubernetesExample.sln --packages /usr/packages 
COPY . .

RUN dotnet build ApacheIgniteNetKubernetesExample.sln -c Release -o /app

FROM build AS publish
ENV IGNITE_VERSION 2.7.0
RUN dotnet publish ApacheIgniteNetKubernetesExample/ApacheIgniteNetKubernetesExample.csproj -c Release -o /app \
	&& cp /usr/packages/apache.ignite/$IGNITE_VERSION/libs/ /app/libs/ -r 
	
FROM base AS final	

#
# Install Java VM (taken from https://github.com/docker-library/openjdk/blob/7a33416016b60c045cf0ba99e82617ed6c130595/8/jdk/slim/Dockerfile)
#
RUN apt-get update && apt-get install -y --no-install-recommends \
		bzip2 \
		unzip \
		xz-utils \
	&& rm -rf /var/lib/apt/lists/*

# Default to UTF-8 file.encoding
ENV LANG C.UTF-8

# add a simple script that can auto-detect the appropriate JAVA_HOME value
# based on whether the JDK or only the JRE is installed
RUN { \
		echo '#!/bin/sh'; \
		echo 'set -e'; \
		echo; \
		echo 'dirname "$(dirname "$(readlink -f "$(which javac || which java)")")"'; \
	} > /usr/local/bin/docker-java-home \
	&& chmod +x /usr/local/bin/docker-java-home

# do some fancy footwork to create a JAVA_HOME that's cross-architecture-safe
RUN ln -svT "/usr/lib/jvm/java-8-openjdk-$(dpkg --print-architecture)" /docker-java-home
ENV JAVA_HOME /docker-java-home

ENV JAVA_VERSION 8u181
ENV JAVA_DEBIAN_VERSION 8u212-b01-1~deb9u1

# see https://bugs.debian.org/775775
# and https://github.com/docker-library/java/issues/19#issuecomment-70546872
#ENV CA_CERTIFICATES_JAVA_VERSION 20130815ubuntu1

RUN set -ex; \
	\
# deal with slim variants not having man page directories (which causes "update-alternatives" to fail)
	if [ ! -d /usr/share/man/man1 ]; then \
		mkdir -p /usr/share/man/man1; \
	fi; \
	\
	apt-get update; \
	apt-get install -y --no-install-recommends \
		openjdk-8-jdk-headless="$JAVA_DEBIAN_VERSION" \
		ca-certificates-java \
	; \
	rm -rf /var/lib/apt/lists/*; \
	\
# verify that "docker-java-home" returns what we expect
	[ "$(readlink -f "$JAVA_HOME")" = "$(docker-java-home)" ]; \
	\
# update-alternatives so that future installs of other OpenJDK versions don't change /usr/bin/java
	update-alternatives --get-selections | awk -v home="$(readlink -f "$JAVA_HOME")" 'index($3, home) == 1 { $2 = "manual"; print | "update-alternatives --set-selections" }'; \
# ... and verify that it actually worked for one of the alternatives we care about
	update-alternatives --query java | grep -q 'Status: manual'

# see CA_CERTIFICATES_JAVA_VERSION notes above
RUN /var/lib/dpkg/info/ca-certificates-java.postinst configure

#
# Copy the files to the main container
#
WORKDIR /app
COPY --from=publish /app . 
EXPOSE 47500 5100
ENTRYPOINT ["dotnet", "ApacheIgniteNetKubernetesExample.dll"]
