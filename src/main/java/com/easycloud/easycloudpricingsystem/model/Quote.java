package com.easycloud.easycloudpricingsystem.model;

import com.azure.data.tables.models.TableEntity;
import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;
import org.apache.tomcat.util.json.JSONParser;

import java.time.LocalDate;

@NoArgsConstructor
@AllArgsConstructor
@Getter
@Setter
public class Quote {
	public String     id          = "";
	public String     title       = "";
	public String     description = "";
	public LocalDate  date        = LocalDate.now();
	public Object service     = null;
	public Object price       = null;

	public TableEntity ToQuoteTable() {
		String partitionKey = "quote";
		String rowKey       = this.id;

		TableEntity quoteTableEntity = new TableEntity(partitionKey, rowKey);

		quoteTableEntity.addProperty("title", this.title);
		quoteTableEntity.addProperty("description", this.description);
		quoteTableEntity.addProperty("cloud_service", this.service.toString());
		quoteTableEntity.addProperty("price", this.price.toString());

		return quoteTableEntity;
	}

	public void ToQuoteObject(TableEntity tableEntity) {
		this.id          = tableEntity.getRowKey();
		this.title       = (String) tableEntity.getProperty("title");
		this.description = (String) tableEntity.getProperty("description");
		this.date        = tableEntity.getTimestamp().toLocalDate();
		this.service     = tableEntity.getProperty("service");
		this.price       = tableEntity.getProperty("price");
	}
}

