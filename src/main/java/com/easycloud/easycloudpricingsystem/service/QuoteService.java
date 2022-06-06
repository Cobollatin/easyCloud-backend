package com.easycloud.easycloudpricingsystem.service;

import com.easycloud.easycloudpricingsystem.model.Quote;
import com.easycloud.easycloudpricingsystem.repository.QuoteRepository;
import lombok.RequiredArgsConstructor;
import org.springframework.stereotype.Service;

import java.time.LocalDate;
import java.util.List;
import java.util.Optional;

@Service
@RequiredArgsConstructor
public class QuoteService {
	private final QuoteRepository quoteRepository;

	public Optional<Quote> add(Quote quote) {
		return quoteRepository.addQuote(quote);
	}

	public Optional<Object> get(String userId, List<String> providersId, LocalDate startDate, LocalDate endDate) {
		return quoteRepository.getQuotes(userId, providersId, startDate, endDate);
	}
}
