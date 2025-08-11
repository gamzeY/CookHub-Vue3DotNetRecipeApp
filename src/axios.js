import axios from 'axios'

const list = axios.create({
    baseURL: 'http://localhost:7001/',
  });

  export default list;