package com.easycloud.easycloudpricingsystem.model;

import com.azure.data.tables.models.TableEntity;
import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;

import java.time.LocalDate;

@NoArgsConstructor
@AllArgsConstructor
@Getter
@Setter
public class Quote {
	public String    id       = "";
	public String    user     = "";
	public String    provider = "";
	public LocalDate date     = LocalDate.now();
	public Object    service  = null;
	public Object    price    = null;

	public Quote(TableEntity tableEntity) {
		this.id       = tableEntity.getPartitionKey();
		this.user     = tableEntity.getRowKey();
		this.provider = tableEntity.getProperty("provider").toString();
		this.date     = tableEntity.getTimestamp().toLocalDate();
		this.service  = tableEntity.getProperty("service");
		this.price    = tableEntity.getProperty("price");
	}

	public TableEntity ToQuoteTable() {
		String partitionKey = this.id;
		String rowKey       = this.user;

		TableEntity quoteTableEntity = new TableEntity(partitionKey, rowKey);

		quoteTableEntity.addProperty("provider", this.service.toString());
		quoteTableEntity.addProperty("service", this.service.toString());
		quoteTableEntity.addProperty("price", this.price.toString());

		return quoteTableEntity;
	}

	public void ToQuoteObject(TableEntity tableEntity) {
		this.id       = tableEntity.getPartitionKey();
		this.user     = tableEntity.getRowKey();
		this.provider = tableEntity.getProperty("provider").toString();
		this.date     = tableEntity.getTimestamp().toLocalDate();
		this.service  = tableEntity.getProperty("service");
		this.price    = tableEntity.getProperty("price");
	}
}

