package com.easycloud.easycloudpricingsystem.repository;

import com.easycloud.easycloudpricingsystem.model.abs.VirtualMachine;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

@Repository
public interface VirtualMachineRepository extends JpaRepository<VirtualMachine, Long> {
}
