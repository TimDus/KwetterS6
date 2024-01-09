import axios from 'axios';
import { getAddress } from './address';

const address = getAddress();

export default axios.create({
    baseURL: `${address}/api`
});