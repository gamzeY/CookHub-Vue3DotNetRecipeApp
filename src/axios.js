import axios from 'axios'

const list = axios.create({
    baseURL: 'https://localhost:7001/',
  });

  export default list;