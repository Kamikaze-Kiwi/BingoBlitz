import http from 'k6/http';
import { check, sleep } from 'k6';

export const options = {
  stages: [
    { duration: '1m', target: 1000 }, // simulate ramp-up of traffic from 1 to 1000 users over 1 minute
    { duration: '2m', target: 1000 }, // stay at 1000 users for 2 minutes
    { duration: '1m', target: 0 }, // ramp-down to 0 users
  ],
  insecureSkipTLSVerify: true,
  noConnectionReuse: true
};

export default function() {
  const res = http.get('https://communityhub.bingoblitz.local/api/objectives/collections/getqueryable?count=5');
  check(res, { 'status was 200': (r) => r.status == 200 });
  sleep(1); 
}
