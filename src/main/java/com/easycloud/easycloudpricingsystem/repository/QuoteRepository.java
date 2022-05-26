package com.easycloud.easycloudpricingsystem.repository;

import com.azure.data.tables.TableClient;
import com.azure.data.tables.TableClientBuilder;
import com.azure.data.tables.TableServiceClient;
import com.azure.data.tables.TableServiceClientBuilder;
import com.azure.data.tables.models.TableEntity;
import com.easycloud.easycloudpricingsystem.model.Quote;

import lombok.extern.slf4j.Slf4j;
import org.springframework.stereotype.Repository;

import java.util.UUID;

@Slf4j
@Repository
public class QuoteRepository {
	private static final String tableName = "quotes";

	//@Value(value = "${connectionString}")
	private final String connectionString ="DefaultEndpointsProtocol=https;AccountName=easycloudstorage;AccountKey=IblbKQ2NW9W/BHGE/niEBbjwu/ejyqBa7wYh6WSB25QR0h0V2kGYtG/zP4xpQoxg8ywZnc4wx27F+AStU83//A==;EndpointSuffix=core.windows.net";

	QuoteRepository() {
		try {
			TableServiceClient tableServiceClient = new TableServiceClientBuilder().connectionString(
					connectionString).buildClient();
			TableClient tableClient = tableServiceClient.createTableIfNotExists(tableName);

		} catch (Exception e) {
			log.error(e.getMessage());
		}
	}

	public Quote addQuote(Quote quote) {
		try {
			TableClient tableClient = new TableClientBuilder().connectionString(connectionString).tableName(
					tableName).buildClient();
			quote.id = UUID.randomUUID().toString();

			TableEntity newQuote = quote.ToQuoteTable();
			tableClient.createEntity(newQuote);
			log.info("Quote created successfully!");
			return quote;
		} catch (Exception e) {
			log.error(e.getMessage());
			return null;
		}
	}

	public Quote getQuote(String id) {
		try {
			TableClient tableClient = new TableClientBuilder().connectionString(connectionString).tableName(
					tableName).buildClient();
			TableEntity specificEntity = tableClient.getEntity(tableName, id);
			if (specificEntity == null) {
				log.warn("Quote doesnt exists!");
				return null;
			}
			Quote quote = new Quote();
			quote.ToQuoteObject(specificEntity);
			log.info("Quote retrieved!");
			return quote;
		} catch (Exception e) {
			log.error(e.getMessage());
			return null;
		}
	}

	public Quote updateQuote(Quote quote) {
		try {
			TableClient tableClient = new TableClientBuilder().connectionString(connectionString).tableName(
					tableName).buildClient();

			TableEntity updatedQuote = tableClient.getEntity(tableName, quote.id);

			if (updatedQuote == null) {
				log.warn("Quote doesnt exists!");
				return null;
			}
			updatedQuote = quote.ToQuoteTable();
			tableClient.upsertEntity(updatedQuote);
			log.info("Quote updated successfully!");
			return quote;
		} catch (Exception e) {
			log.error(e.getMessage());
			return null;
		}
	}

}
