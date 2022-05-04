package com.easycloud.easycloudpricingsystem.model.abs;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.Id;
import javax.persistence.Table;

@Entity
@Table
public class CloudService {
	@Id
	@Column(name = "id", nullable = false)
	private Long id;
}
