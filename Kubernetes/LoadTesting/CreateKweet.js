import http from 'k6/http';
import { check, sleep } from 'k6';

export let options = {
    stages: [
        { duration: '10s', target: 10 }, // simulate ramp-up of traffic from 1 to 100 users over 1 minute.
        { duration: '10s', target: 10 }, // stay at 100 users for 2 minutes
        { duration: '10s', target: 0 },   // ramp-down to 0 users
    ],
    thresholds: {
        http_req_duration: ['avg < 400', 'p(99) < 2000'],
        checks: ['rate>0.9']
    }
};

function kweetCreate() {
    const url = "http://kweetservice.api:8102/api/kweet/create";
    const payload = JSON.stringify({ "customerId": 1, "text": "today is a nice day" });
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
    kweetCreate();
    sleep(1);
}