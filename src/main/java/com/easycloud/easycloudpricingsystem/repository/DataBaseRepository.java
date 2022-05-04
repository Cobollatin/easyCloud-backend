package com.easycloud.easycloudpricingsystem.repository;

import com.easycloud.easycloudpricingsystem.model.abs.DataBase;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

@Repository
public interface DataBaseRepository extends JpaRepository<DataBase, Long> {
}
