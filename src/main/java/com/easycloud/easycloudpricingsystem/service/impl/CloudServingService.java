package com.easycloud.easycloudpricingsystem.service.impl;

import com.easycloud.easycloudpricingsystem.repository.DataBaseRepository;
import com.easycloud.easycloudpricingsystem.service.abs.CloudServiceService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

@Service
public class CloudServingService implements CloudServiceService {
	@Autowired
	private DataBaseRepository dataBaseRepository;
}
