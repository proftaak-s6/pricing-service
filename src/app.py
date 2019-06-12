from flask import Flask, request, jsonify
from db import get_all, get, create, update
from datetime import datetime

import json

app = Flask(__name__)


@app.route('/prices', methods=["GET"])
def get_all_locations():
    locations = get_all()

    if not locations:
        return 204

    return jsonify(locations)


@app.route('/prices/<name>', methods=["GET"])
def get_location(name: str):
    location = get(name)

    if not location:
        location = create(name, datetime.now())

    return jsonify(location)


@app.route('/prices/<name>/<time_string>', methods=["GET"])
def get_prices_for_location_at_time(name: str, time_string: str):
    location = get(name)

    if not location:
        location = create(name, datetime.now())
        price = next(iter(location['history']), {"price": 1})['price']
    else:
        # Zoek de dichtsbijzijnde
        price = 2

    return jsonify({"price": price})


@app.route('/prices/<name>', methods=["PUT"])
def update_location(name: str):
    location = get(name)
    new_price = float(request.data.decode()) if float(
        request.data.decode()) > 0 else 1
    print(new_price, flush=True)

    if not location:
        create(name, datetime.now(), new_price)

    update(name, new_price)

    return "", 203


if __name__ == "__main__":
    app.run(host="0.0.0.0", debug=False)
