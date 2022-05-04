package com.easycloud.easycloudpricingsystem.model.abs;

import lombok.AllArgsConstructor;
import lombok.NoArgsConstructor;

import javax.persistence.*;

@Entity
@Table(name = "Quote")
@NoArgsConstructor
@AllArgsConstructor
public class Quote {
	@Transient
	private String apiUrl;

	@Transient
	private String apiRoute;

	@Column(name = "provider", nullable = false, updatable = false)
	private String provider;

	@ManyToOne(fetch = FetchType.LAZY)
	@JoinColumn(name = "serviceId")
	private CloudService serviceId;

	@Id
	@GeneratedValue(strategy = GenerationType.IDENTITY)
	@Column(name = "id", nullable = false)
	private Long id;

	public void Compute(){

	};
}
