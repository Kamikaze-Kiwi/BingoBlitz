import http from 'k6/http';
import { check, sleep } from 'k6';

export const options = {
  stages: [
    { duration: '30s', target: 400 }, // simulate ramp-up of traffic from 1 to 400 users over 30 seconds,
    { duration: '1m', target: 400 }, // stay at 300 users for 2 minutes
    { duration: '30s', target: 0 }, // ramp-down to 0 users
  ],
  insecureSkipTLSVerify: true,
  noConnectionReuse: true
};

export default function() {
  const res = http.post('https://gameserver.bingoblitz.local/api/game/join?gameId=123456');
  check(res, { 'status was 200': (r) => r.status == 200 });
  sleep(1); 
}
