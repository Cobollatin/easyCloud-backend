package com.easycloud.easycloudpricingsystem;

import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.web.servlet.config.annotation.EnableWebMvc;
import springfox.documentation.builders.PathSelectors;
import springfox.documentation.builders.RequestHandlerSelectors;
import springfox.documentation.spi.DocumentationType;
import springfox.documentation.spring.web.plugins.Docket;

@SpringBootApplication
public class EasyCloudPricingSystemApplication {
	public static void main(String[] args) {
		SpringApplication.run(EasyCloudPricingSystemApplication.class, args);
	}

	@Configuration
	@EnableWebMvc
	public static class SwaggerConfig {
		@Bean
		public Docket api() {
			return new Docket(DocumentationType.SWAGGER_2).select().apis(
					RequestHandlerSelectors.basePackage("com.easycloud.easycloudpricingsystem.controller")).apis(
					RequestHandlerSelectors.any()).paths(PathSelectors.any()).build();
		}
	}
}
