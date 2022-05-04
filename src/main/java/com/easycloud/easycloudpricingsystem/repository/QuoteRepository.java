package com.easycloud.easycloudpricingsystem.repository;

import com.easycloud.easycloudpricingsystem.model.abs.Quote;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

@Repository
public interface QuoteRepository extends JpaRepository<Quote, Long> {
}
