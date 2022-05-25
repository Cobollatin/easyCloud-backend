package com.easycloud.easycloudpricingsystem.model;

import com.azure.data.tables.models.TableEntity;
import com.fasterxml.jackson.databind.util.JSONPObject;
import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;

import java.util.Date;
import java.util.HashMap;
import java.util.Map;

@NoArgsConstructor
@AllArgsConstructor
@Getter
@Setter
public class Quote {
	public String      Id;
	public String      Title;
	public String      Description;
	public Date        Date;
	public JSONPObject CloudService;
	public JSONPObject Price;

	public TableEntity ToQuoteTable(){
		String partitionKey = "quote";
		String rowKey       = this.Id;

		Map<String, Object> quoteInfo = new HashMap<>();
		quoteInfo.put("title", this.Title);
		quoteInfo.put("description", this.Description);
		quoteInfo.put("date", this.Date);
		quoteInfo.put("cloud_service", this.CloudService);
		quoteInfo.put("price", this.Price);

		return new TableEntity(partitionKey, rowKey).setProperties(quoteInfo);
	}

	public void ToQuoteObject(TableEntity tableEntity){
		this.Id = tableEntity.getRowKey();
		this.Title = (String) tableEntity.getProperty("title");
		this.Description = (String) tableEntity.getProperty("description");
		this.Date = (Date) tableEntity.getProperty("date");
		this.CloudService = (JSONPObject) tableEntity.getProperty("cloud_service");
		this.Price = (JSONPObject) tableEntity.getProperty("price");
	}
}

