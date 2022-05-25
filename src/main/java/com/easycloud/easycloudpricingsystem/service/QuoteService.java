package com.easycloud.easycloudpricingsystem.service;

import com.easycloud.easycloudpricingsystem.repository.QuoteRepository;
import lombok.RequiredArgsConstructor;
import org.springframework.stereotype.Service;

import java.lang.annotation.Annotation;

@Service
@RequiredArgsConstructor
public class QuoteService  {
	private final QuoteRepository quoteRepository;


}
