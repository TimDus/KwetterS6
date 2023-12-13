import http from 'k6/http';

export default function () {

    const url = "http://kwetter.localhost:9080/api/Kweet/Create";
    const payload = JSON.stringify({ "customerId": 1, "text": 'today is a nice day', });
    const params = {
        headers: {
            'Content-Type': 'application/json',
            'Connection': 'keep-alive',
            'Accept': '*/*',
            'Accept-Encoding': 'gzip, deflate, br'
        },
    };

    http.post(url, payload, params);
}