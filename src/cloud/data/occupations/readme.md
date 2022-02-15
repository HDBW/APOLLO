#Adventures in scraping the bibb 

The purpose of the Bibb crawler is to identify and map occupations as defined in KLDB. However the Bibb crawler will call the referenced data stores the values for further processing.

Please note the bibb crawler is an essential component in the distributed data aggregation system (dags). 
Its main purpose is to identify the jobs relevant for the scope of "INVITE project apollo".

First thing to notice is type of usability of the bibb php site. It lacks some features while focusing on others. An implementation for visually impared users would have made our live much easier. However there are several accessibility features of this site. 

## Facts

### Facts - the bibb search for apprenticeship (Ausbildung)

[bibb apprenticeship search](https://www.bibb.de/dienst/berufesuche/de/index_berufesuche.php/apprenticeships/?)

The bibb website as of february 2022 contains search results in the div:
``<main class="fr-main-column">`` which contains the container div as well as
``<div class="col-lg-8">``. In there each acknowledged occupation in the scope of dual education is represented by a paragraph. 
However, the search implementation is a form submit button. So we choose to use the fixed URL instead.
This will result in an alphabetical list of occupations. 

After anchors and divs for the alphabetical index we find a div containing the content as well as a list of occupations.
```
<div class="c-accentuation-box c-accentuation-box--with-links">
                                    <div class="c-accentuation-box__content">
                                        <ul>                                                                                            
                                            <li>
                                                <a href="http://www.bibb.de/dienst/berufesuche/de/index_berufesuche.php/profile/apprenticeship/nmhjbkhi">Änderungsschneider/ Änderungsschneiderin (Handwerk, Industrie und Handel)</a>
                                            </li>
                                            <li>
                                                <a href="http://www.bibb.de/dienst/berufesuche/de/index_berufesuche.php/profile/apprenticeship/210715">Anlagenmechaniker für Sanitär-, Heizungs- und Klimatechnik/ Anlagenmechanikerin für Sanitär-, Heizungs- und Klimatechnik (Handwerk, Industrie und Handel)</a>
                                            </li>
                                            ...                                                                                                                                                                                                                                                    
                                        </ul>
                                    </div>
                                </div>
```



##Related Services

