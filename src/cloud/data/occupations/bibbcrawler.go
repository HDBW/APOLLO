/*
(c) 2022, Hochschule der Bayerischen Wirtschaft gGmbH
This crawler allows us to collect the occupations as stated on the bibb websites:
[bibb apprenticeship search](https://www.bibb.de/dienst/berufesuche/de/index_berufesuche.php/search)

*/
package main

import (
	"fmt"
	"github.com/gocolly/colly"
)

type Fact struct {
	//TODO: Define Fact struct for bibb
}

func main() {
	//create factlist
	//allFacts := make([]Fact, 0)

	//TODO: crawl the list of occupations relevant for INVITE project apollo

	//create instance of colly collector
	collectoccupations := colly.NewCollector(
		//TODO: Add list of external references of bibb
		colly.AllowedDomains("bibb.de"),
	)

	collectoccupations.OnHTML("c-link-block__link", func(element *colly.HTMLElement) {

	})

	collectoccupations.OnRequest(func(request *colly.Request) {
		fmt.Println("Visiting: ", request.URL.String())
	})

	//TODO: create search URLs
	collectoccupations.Visit("https://www.bibb.de/dienst/berufesuche/de/index_berufesuche.php/apprenticeships/?")

	//TODO: crawl all the occupations on the list
}
