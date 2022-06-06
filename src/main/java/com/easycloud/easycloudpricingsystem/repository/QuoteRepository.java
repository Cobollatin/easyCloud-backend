package com.easycloud.easycloudpricingsystem.repository;

import com.azure.data.tables.TableClient;
import com.azure.data.tables.TableClientBuilder;
import com.azure.data.tables.TableServiceClient;
import com.azure.data.tables.TableServiceClientBuilder;
import com.azure.data.tables.models.ListEntitiesOptions;
import com.azure.data.tables.models.TableEntity;
import com.easycloud.easycloudpricingsystem.model.Quote;
import lombok.extern.slf4j.Slf4j;
import org.springframework.stereotype.Repository;

import java.sql.Timestamp;
import java.time.LocalDate;
import java.util.ArrayList;
import java.util.List;
import java.util.Optional;
import java.util.UUID;

@Slf4j
@Repository
public class QuoteRepository {
	private static final String tableName = "quotes";

	//@Value(value = "${connectionString}")
	private final String connectionString = "DefaultEndpointsProtocol=https;AccountName=easycloudstorage;" +
			"AccountKey=IblbKQ2NW9W/BHGE/niEBbjwu/ejyqBa7wYh6WSB25QR0h0V2kGYtG/zP4xpQoxg8ywZnc4wx27F+AStU83//A==;" +
			"EndpointSuffix=core.windows.net";

	QuoteRepository() {
		try {
			TableServiceClient tableServiceClient = new TableServiceClientBuilder().connectionString(
					connectionString).buildClient();
			TableClient tableClient = tableServiceClient.createTableIfNotExists(tableName);
		} catch (Exception e) {
			log.error(e.getMessage());
		}
	}

	public Optional<Quote> addQuote(Quote quote) {
		try {
			TableClient tableClient = new TableClientBuilder().connectionString(connectionString).tableName(
					tableName).buildClient();
			quote.setId(UUID.randomUUID().toString());
			TableEntity newQuote = quote.ToQuoteTable();
			tableClient.createEntity(newQuote);
			log.info("Quote created successfully!");
			return Optional.of(quote);
		} catch (Exception e) {
			log.error(e.getMessage());
			return Optional.empty();
		}
	}

	public Optional<Object> getQuotes(
			String userId, List<String> providersId, LocalDate startDate, LocalDate endDate
	) {
		try {
			TableClient tableClient = new TableClientBuilder().connectionString(connectionString).tableName(
					tableName).buildClient();

			Timestamp start = Timestamp.valueOf(startDate.atStartOfDay());
			Timestamp end   = Timestamp.valueOf(endDate.atTime(23, 59, 59));

			StringBuilder filter = new StringBuilder(
					"PartitionKey eq '" + userId + "' and " + "Timestamp ge datetime'" + start + "' and " + "Timestamp" +
							" " + "le datetime'" + end + "'");

			filter.append(" and (");
			for (int i = 0; i < providersId.size(); i++) {
				filter.append("provider eq '").append(providersId.get(i)).append("'");
				if (i < providersId.size() - 1) {
					filter.append(" or ");
				}
			}
			filter.append(")");

			ListEntitiesOptions options = new ListEntitiesOptions().setFilter(filter.toString());

			List<Quote> quotes = new ArrayList<Quote>();
			tableClient.listEntities(options, null, null).forEach((TableEntity entity) -> {
				quotes.add(new Quote(entity));
			});

			if (quotes.isEmpty()) {
				log.warn("Quotes doesnt exists!");
				return Optional.empty();
			}
			log.info("Quotes retrieved!");
			return Optional.of(quotes);
		} catch (Exception e) {
			log.error(e.getMessage());
			return Optional.empty();
		}
	}
}
