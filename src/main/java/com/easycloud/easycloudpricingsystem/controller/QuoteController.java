package com.easycloud.easycloudpricingsystem.controller;

import com.easycloud.easycloudpricingsystem.model.Quote;
import com.easycloud.easycloudpricingsystem.service.QuoteService;
import lombok.extern.slf4j.Slf4j;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.time.LocalDate;
import java.time.format.DateTimeFormatter;
import java.util.Arrays;
import java.util.HashMap;
import java.util.Map;
import java.util.Optional;

@Slf4j
@RestController
@RequestMapping("/quote")

public class QuoteController {

	@Autowired
	private QuoteService quoteService;

	@PostMapping(value = "/add/{userId}/{providerId}")
	public ResponseEntity<Map<String, Object>> addQuote(
			@PathVariable String userId, @PathVariable String providerId, @RequestBody Quote quote
	) {
		try {
			quote.setUser(userId);
			quote.setProvider(providerId);
			return __HANDLE_RESPONSE__(Optional.of(quoteService.add(quote).get()), "Invalid object type");
		} catch (Exception e) {
			log.error(e.getMessage());
			return ResponseEntity.internalServerError().build();
		}
	}

	@GetMapping(value = "/get/{userId}")
	public ResponseEntity<Map<String, Object>> getQuote(
			@PathVariable String userId, @RequestBody Map<String, Object> params
	) {
		try {
			DateTimeFormatter df = DateTimeFormatter.ofPattern("dd-MM-yyyy");
			Optional<Object> quotes = quoteService.get(userId, Arrays.asList(params.get("providers").toString().split(
					                                           ",")),
			                                           LocalDate.parse(params.get("start").toString(), df),
			                                           LocalDate.parse(params.get("end").toString(), df));
			if (quotes.isPresent()) {
				return __HANDLE_RESPONSE__(quotes, "Invalid object type");
			} else {
				return ResponseEntity.notFound().build();
			}
		} catch (Exception e) {
			log.error(e.getMessage());
			return ResponseEntity.internalServerError().build();
		}
	}

	private ResponseEntity<Map<String, Object>> __HANDLE_RESPONSE__(Optional<Object> object, String message) {
		Map<String, Object> res = new HashMap<>();
		if (object.isEmpty()) {
			res.put("status", "fail");
			res.put("message", message);
			return ResponseEntity.ok(res);
		}
		res.put("status", "success");
		res.put("payload", object);
		return ResponseEntity.ok(res);
	}

}
