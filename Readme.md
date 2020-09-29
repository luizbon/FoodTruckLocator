# Design Decisions

- The Permit model contains only the data needed to find (Lat/Long/ExpirationDate) and let the user choose the food truck (Name/Address/FoodItems/DaysHours)

- Reading the documentation I found the dataset is published daily with weekly updates, so the retrieval of a new dataset will be based on this.

- A class library is used to hold most of the logic for future code reusability. ie. Create a CLI or ChatBot interface