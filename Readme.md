# Design Decisions

- The Permit model contains only the data needed to find (Lat/Long/ExpirationDate) and let the user choose the food truck (Name/Address/FoodItems/DaysHours)
- Reading the documentation I found the dataset is published daily with weekly updates, so the retrieval of a new dataset will be based on this.
- A class library is used to hold most of the logic for future code reusability. ie. Create a CLI or ChatBot interface
- Use regular docker user for security
- Docker uses only HTTP for simplicity

## Available improvements

- Better request validation
- Let user choose how many results to return
- Use a cloud blob storage
- Add proper logging
- Integrate with external configuration system (ie. App Cofiguration) and enable dynamic updates
- Use distributed cache
- Add PubSub system to invalidate the cache when the background doanloader updates the CSV file
- Use local time to identify if permit is expired
- Filter closed food trucks based on 'DaysHours'
- Dump CSV file on a Document Datbase
- Write integration tests on the Web server
- Implement a Map page and use device GPS to identify where the user is
- CI/CD
- Controller caching