package com.easycloud.easycloudpricingsystem.controller;

import com.easycloud.easycloudpricingsystem.model.Quote;
import com.easycloud.easycloudpricingsystem.service.QuoteService;
import lombok.extern.slf4j.Slf4j;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.HashMap;
import java.util.Map;

@Slf4j
@RestController
@RequestMapping("/quote")

public class QuoteController {

	@Autowired
	private QuoteService quoteService;

	@PostMapping(value = "/add")
	public ResponseEntity<Map<String, Object>> addQuote(@RequestBody Quote quote) {
		try {
			quote = quoteService.add(quote);
			return __HANDLE_RESPONSE__(quote, "Invalid object type");
		} catch (Exception e) {
			log.error(e.getMessage());
			return ResponseEntity.internalServerError().build();
		}
	}

	@GetMapping(value = "/get/{id}")
	public ResponseEntity<Map<String, Object>> getQuote(@PathVariable String id) {
		try {
			Quote quote = quoteService.get(id);
			return __HANDLE_RESPONSE__(quote, "Quote not found");
		} catch (Exception e) {
			log.error(e.getMessage());
			return ResponseEntity.internalServerError().build();
		}
	}

	@PostMapping(value = "/update")
	public ResponseEntity<Map<String, Object>> updateQuote(@RequestBody Quote quote) {
		try {
			quote = quoteService.update(quote);
			return __HANDLE_RESPONSE__(quote, "Quote not found");
		} catch (Exception e) {
			log.error(e.getMessage());
			return ResponseEntity.internalServerError().build();
		}
	}

	private ResponseEntity<Map<String, Object>> __HANDLE_RESPONSE__(Quote object, String message) {
		Map<String, Object> res = new HashMap<>();
		if (object == null) {
			res.put("status", "fail");
			res.put("message", message);
			return ResponseEntity.ok(res);
		}
		res.put("status", "success");
		res.put("payload", object);
		return ResponseEntity.ok(res);
	}

}
