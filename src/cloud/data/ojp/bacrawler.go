/*
Copyright (c) 2022 Hochschule der Bayerischen Wirtschaft gGmbH
The BA crawler is used to search the "BA" API for a specific job
We are currently collecting data for all 16 states however data processing is currently limited to
the states of "Bavaria" and "Baden-Wurttemberg".

This crawler is written against the BA API thanks to the amazing work of:
- [Lilith Wittmann](https://github.com/LilithWittmann)
- [Dr. Andreas Fischer](https://github.com/AndreasFischer1985)
*/
package main

import (
	"fmt"
	"github.com/gocolly/colly"
)

//TODO: https://github.com/bundesAPI/jobsuche-api

//TODO: https://jorin.me/use-go-channels-to-build-a-crawler/

//TODO: https://github.com/nats-io/nats.go

func main() {
	c := colly.NewCollector()

	// Find and visit all links
	c.OnHTML("a[href]", func(e *colly.HTMLElement) {
		err := e.Request.Visit(e.Attr("href"))
		if err != nil {
			return
		}
	})

	c.OnRequest(func(r *colly.Request) {
		fmt.Println("Visiting", r.URL)
	})

	err := c.Visit("http://go-colly.org/")
	if err != nil {
		return
	}
}
