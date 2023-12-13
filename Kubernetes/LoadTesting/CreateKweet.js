import http from 'k6/http';
import { check, sleep } from 'k6';

export const GATEWAY_API_URL = "http://kwetter.localhost:9080/api";

export let options = {
    stages: [
        { duration: '1m', target: 100 }, // simulate ramp-up of traffic from 1 to 100 users over 1 minute.
        { duration: '2m', target: 100 }, // stay at 100 users for 2 minutes
        { duration: '1m', target: 0 },   // ramp-down to 0 users
    ],
    thresholds: {
        http_req_duration: ['avg < 400', 'p(99) < 2000'],
        checks: ['rate>0.9']
    }
};

function verifyUserNameUniqueness() {
    const url = `${GATEWAY_API_URL}/Kweet/Create`;
    const payload = JSON.stringify({ "customerId": 1, "text": "Today is a nice day" });
    const params = {
        headers: {
            'Content-Type': 'application/json'
        },
    };
    const res = http.post(url, payload, params);
    check(res, {
        'status is 200': (r) => r.status == 200,
    });
}

export default function () {
    verifyUserNameUniqueness();
    sleep(1);
}