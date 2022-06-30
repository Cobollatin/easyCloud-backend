FROM openjdk:17-jdk-alpine
RUN apk update && apk add git && apk add sudo
RUN git clone https://github.com/UPC-2022-1/easyCloud-backend
RUN cd easyCloud-backend \
    && git checkout easyCloud-Java-Microservice-Pricing-System-develop
RUN sudo chmod -R 777 ./easyCloud-backend
RUN cd easyCloud-backend && ./gradlew build  \
    && java -jar ./build/libs/easyCloud-Pricing-System-0.0.1-SNAPSHOT.jar
EXPOSE 9999