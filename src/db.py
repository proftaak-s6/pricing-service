from datetime import datetime

locations = []
standard_price = 1


def get_all():
    return locations


def get(location_name: str):
    for location in locations:
        if location['location'].lower() == location_name.lower():
            return location
    else:
        return None


def create(location_name: str, date: datetime, price: float = standard_price):
    location = {
        "location": location_name,
        "history": [
            {
                "date": date,
                "price": standard_price
            }
        ]
    }

    locations.append(location)

    return location


def update(location_name: str, price: float, date: datetime = datetime.now()):
    for location in locations:
        if location['location'].lower() == location_name.lower():
            location['history'].append({
                "date": date,
                "price": price
            })  
    
