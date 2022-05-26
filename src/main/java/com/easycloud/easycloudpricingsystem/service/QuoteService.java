package com.easycloud.easycloudpricingsystem.service;

import com.easycloud.easycloudpricingsystem.model.Quote;
import com.easycloud.easycloudpricingsystem.repository.QuoteRepository;
import lombok.RequiredArgsConstructor;
import org.springframework.stereotype.Service;

@Service
@RequiredArgsConstructor
public class QuoteService {
	private final QuoteRepository quoteRepository;

	public Quote add(Quote quote) {
		return quoteRepository.addQuote(quote);
	}

	public Quote get(String id) {
		return quoteRepository.getQuote(id);
	}

	public Quote update(Quote quote) {
		return quoteRepository.updateQuote(quote);
	}

}
