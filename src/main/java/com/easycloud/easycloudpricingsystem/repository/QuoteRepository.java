package com.easycloud.easycloudpricingsystem.repository;

import com.azure.data.tables.TableClient;
import com.azure.data.tables.TableClientBuilder;
import com.azure.data.tables.models.TableEntity;
import com.easycloud.easycloudpricingsystem.model.Quote;

import java.util.HashMap;
import java.util.Map;
import java.util.UUID;

public class QuoteRepository {
	static String connectionString = System.getenv("ConnectionString");

	public static Map<String, Object> addQuote(Quote quote) {
		try {
			// Create a TableClient with a connection string and a table name.
			TableClient tableClient = new TableClientBuilder().connectionString(connectionString).tableName(
					"quotes").buildClient();

			// Create a new employee TableEntity.
			quote.Id = UUID.randomUUID().toString();

			while (tableClient.getEntity("quote", quote.Id) != null) {
				quote.Id = UUID.randomUUID().toString();
			}

			TableEntity newQuote = quote.ToQuoteTable();

			// Upsert the entity into the table
			tableClient.upsertEntity(newQuote);

			Map<String, Object> res = new HashMap<>();

			res.put("status", "success");
			res.put("payload", quote);

			return res;
		} catch (Exception e) {
			// Output the stack trace.
			e.printStackTrace();
			Map<String, Object> res = new HashMap<>();

			res.put("status", "error");
			res.put("payload", e.getMessage());

			return res;
		}
	}

	public static Map<String, Object> getQuote(String id) {
		try {
			final String tableName = "Employees";

			// Create a TableClient with a connection string and a table name.
			TableClient tableClient = new TableClientBuilder().connectionString(connectionString).tableName(
					tableName).buildClient();

			// Get the specific entity.
			TableEntity specificEntity = tableClient.getEntity("quote", id);

			Map<String, Object> res = new HashMap<>();
			res.put("status", "success");

			// Output the entity.
			if (specificEntity == null) {
				res.put("payload", "Quote doesnt exists.");
				return res;
			}

			Quote quote = new Quote();
			quote.ToQuoteObject(specificEntity);

			res.put("payload", quote);

			return res;
		} catch (Exception e) {
			// Output the stack trace.
			e.printStackTrace();
			Map<String, Object> res = new HashMap<>();

			res.put("status", "error");
			res.put("message", e.getMessage());

			return res;
		}
	}

}
