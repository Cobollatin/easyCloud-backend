package com.easycloud.easycloudpricingsystem.service.impl;

import com.easycloud.easycloudpricingsystem.repository.QuoteRepository;
import com.easycloud.easycloudpricingsystem.service.abs.QuoteService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

@Service
public class QuotingService implements QuoteService {
	@Autowired
	private QuoteRepository quoteRepositor;
}
