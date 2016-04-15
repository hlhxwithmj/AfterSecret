angular.module("constantService", [])
     .service('constantService', [function () {
         this.nationality = ['Algerian', 'Angolan', 'Bangladeshi', 'British', 'Albanian', 'Bosnian', 'Botswanan',
             'Bahamian', 'Belgian', 'Belizean', 'Brazilian', 'Bahraini', 'Andorran', 'Beninese', 'Bhutanese', 'Barbadian',
             'Australian', 'Argentinian', 'Bolivian', 'Belarusian', 'Bruneian', 'Armenian', 'Azerbaijani', 'Afghan',
             'Austrian', 'Bulgarian', 'Burkinese', 'Burmese', 'Burundian', 'Cambodian', 'Cameroonian', 'Canadian',
             'Cape Verdean', 'Chadian', 'Chilean', 'China Hong Kong', 'China Macau', 'Chinese', 'Colombian', 'Congolese',
             'Costa Rican', 'Croatian', 'Cuban', 'Cypriot', 'Czech', 'Danish', 'Djiboutian', 'Dominican', 'Dominican',
             'Dutch', 'Ecuadorean', 'Egyptian', 'Emirates', 'English', 'Eritrean', 'Estonian', 'Ethiopian', 'Fijian',
             'Finnish', 'French', 'Gabonese', 'Gambian', 'Georgian', 'German', 'Ghanaian', 'Greek', 'Grenadian',
             'Guatemalan', 'Guinean', 'Guyanese', 'Haitian', 'Honduran', 'Hungarian', 'Icelandic', 'Indian', 'Indonesian',
             'Iranian', 'Iraqi', 'Irish', 'Italian', 'Jamaican', 'Japanese', 'Jordanian', 'Kazakh', 'Kenyan', 'Kuwaiti',
             'Laotian', 'Latvian', 'Lebanese', 'Liberian', 'Libyan', 'Lithuanian', 'Macedonian', 'Madagascan', 'Malawian',
             'Malaysian', 'Maldivian', 'Malian', 'Maltese', 'Mauritanian', 'Mauritian', 'Mexican', 'Moldovan', 'Monacan',
             'Mongolian', 'Montenegrin', 'Moroccan', 'Mozambican', 'Namibian', 'Nepalese', 'New Zealand', 'Nicaraguan',
             'Nigerian', 'Nigerien', 'North Korean', 'Norwegian', 'Omani', 'Pakistani', 'Panamanian', 'Papua New Guinean',
             'Paraguayan', 'Peruvian', 'Philippine', 'Polish', 'Portuguese', 'Qatari', 'Romanian', 'Russian', 'Rwandan',
             'Salvadorean', 'Saudi Arabian', 'Scottish', 'Senegalese', 'Serbian', 'Seychellois', 'Sierra Leonian',
             'Singaporean', 'Slovak', 'Slovenian', 'Somali', 'South African', 'South Korean', 'Spanish', 'Sri Lankan',
             'Sudanese', 'Surinamese', 'Swazi', 'Swedish', 'Swiss', 'Syrian', 'Tadjik', 'Taiwanese', 'Tanzanian', 'Thai',
             'Tobagan', 'Tobagonian', 'Togolese', 'Trinidadian', 'Tunisian', 'Turkish', 'Turkoman', 'Tuvaluan', 'Ugandan',
             'Ukrainian', 'Uruguayan', 'Uzbek', 'Vanuatuan', 'Venezuelan', 'Vietnamese', 'Welsh', 'Western Samoan',
             'Yemeni', 'Yugoslav', 'Zaïrean', 'Zambian', 'Zimbabwean'];
         this.occupation = ['Agriculture', 'Business', 'Chemical industry', 'Communications', 'Construction', 'Designer',
             'Doctor', 'Education', 'Energy industry', 'Entertainment', 'Fashion and Accessories', 'Finacial activities',
             'Government', 'Health care', 'Information', 'IT', 'Leisure and hospitality', 'Manufacturing', 'Mass Media',
             'Military', 'Retail Trade', 'Telecommunications', 'Transportation and warehousing', 'Wholesale trade', 'Others'];
     }]);