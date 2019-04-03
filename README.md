# ignitenet-kubernetes-example
A repro project for https://stackoverflow.com/q/55408181/973457

I've created a public Docker image with the demo application: alexavrutin/ignitenet-kubernetes-example. This repo also contains a Kubernetes deployment file which you can use to get the image deployed in your K8s cluster with this command:

kubectl apply -f .\kubernetes-deployment.yml

This application fails at launch with the same exception like my real application: 

Unhandled Exception: Apache.Ignite.Core.Common.IgniteException: Java exception occurred [class=java.lang.NoSuchFieldError, message=logger] ---> Apache.Ignite.Core.Com
mon.JavaException: java.lang.NoSuchFieldError: logger
        at org.springframework.beans.factory.support.DefaultListableBeanFactory.preInstantiateSingletons(DefaultListableBeanFactory.java:727)
        at org.springframework.context.support.AbstractApplicationContext.finishBeanFactoryInitialization(AbstractApplicationContext.java:867)
        at org.springframework.context.support.AbstractApplicationContext.refresh(AbstractApplicationContext.java:543)
        at org.apache.ignite.internal.util.spring.IgniteSpringHelperImpl.applicationContext(IgniteSpringHelperImpl.java:381)
        at org.apache.ignite.internal.util.spring.IgniteSpringHelperImpl.loadConfigurations(IgniteSpringHelperImpl.java:104)
        at org.apache.ignite.internal.util.spring.IgniteSpringHelperImpl.loadConfigurations(IgniteSpringHelperImpl.java:98)
        at org.apache.ignite.internal.IgnitionEx.loadConfigurations(IgnitionEx.java:751)
        at org.apache.ignite.internal.IgnitionEx.loadConfiguration(IgnitionEx.java:809)
        at org.apache.ignite.internal.processors.platform.PlatformIgnition.configuration(PlatformIgnition.java:153)
        at org.apache.ignite.internal.processors.platform.PlatformIgnition.start(PlatformIgnition.java:68)
 
If you have any question, feel free to contact me by email: alexavrutin !at! gmail.com or using Telegram:@alexavrutin (EN/RU).

Hope it helps.         
