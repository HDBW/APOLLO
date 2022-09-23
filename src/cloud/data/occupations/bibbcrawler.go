/*
(c) 2022, Hochschule der Bayerischen Wirtschaft gGmbH
This crawler allows us to collect the occupations as stated on the bibb websites:
[bibb apprenticeship search](https://www.bibb.de/dienst/berufesuche/de/index_berufesuche.php/search)

Check out colly documentation for more information
https://pkg.go.dev/github.com/gocolly/colly#HTMLElement
*/
package main

import (
	"encoding/json"
	"fmt"
	"github.com/gocolly/colly"
	"log"
	"os"
	"time"
)

//TODO: Declare LimitRule
type LimitRule struct {
	// DomainRegexp is a regular expression to match against domains
	DomainRegexp string
	// DomainRegexp is a glob pattern to match against domains
	DomainGlob string
	// Delay is the duration to wait before creating a new request to the matching domains
	Delay time.Duration
	// RandomDelay is the extra randomized duration to wait added to Delay before creating a new request
	RandomDelay time.Duration
	// Parallelism is the number of the maximum allowed concurrent requests of the matching domains
	Parallelism int
	// contains filtered or unexported fields
}

type BibbOccupations struct {
	//TODO: Define Fact struct for bibb
}

type WebPage struct {
	Url     string `json:"url"`
	Date    string `json:"date"`
	Title   string `json:"title"`
	Content string `json:"content"`
}

func main() {
	//create factlist
	var webPage = WebPage{}

	//TODO: crawl the list of occupations relevant for INVITE project apollo

	//create instance of colly collector
	collectoccupations := colly.NewCollector(
	//TODO: Add list of external references of bibb
	//colly.AllowedDomains("bibb.de"),
	)

	collectoccupations.OnHTML("head title", func(element *colly.HTMLElement) {
		//fmt.Println(element.Text)
		webPage.Title = element.Text

	})

	collectoccupations.OnHTML("html body", func(element *colly.HTMLElement) {
		//fmt.Println(element.Text)
		webPage.Content = element.Text
	})

	collectoccupations.OnHTML("c-accentuation-box__content", func(htmlElement *colly.HTMLElement) {
		fmt.Println("dropping occupations")
		htmlElement.ForEach("li", func(i int, listElement *colly.HTMLElement) {
			println(listElement.Text)
		})
		fmt.Println("done dropping occupations")
	})

	collectoccupations.OnRequest(func(request *colly.Request) {
		webPage.Url = request.URL.String()
		fmt.Println("Visiting: ", request.URL.String())
	})

	//TODO: crawl all the occupations on the list
	//TODO: Create a list of Occupations to crawl -> div class c-accentuation-box__content contains ul with links to occupations
	//get response from website
	collectoccupations.OnResponse(func(response *colly.Response) {
		//TODO: Statistics for AppInsights like pageCount++
		log.Println(response.Headers)
	})

	//done loading index page
	collectoccupations.OnScraped(func(response *colly.Response) {
		//TODO: Store on Azure

		//TODO: Save to specific blob
		enc := json.NewEncoder(os.Stdout)
		enc.SetIndent("", " ")
		enc.Encode(webPage)
	})

	//TODO: create search URLs
	collectoccupations.Visit("https://www.bibb.de/dienst/berufesuche/de/index_berufesuche.php/apprenticeships/?")
}
