package com.easycloud.easycloudpricingsystem.repository;

import com.easycloud.easycloudpricingsystem.model.abs.ServerLess;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

@Repository
public interface ServerLessRepository extends JpaRepository<ServerLess, Long> {
}
