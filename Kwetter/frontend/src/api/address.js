const getIp = () => {
    if (process.env.REACT_APP_API_CUSTOM_IP === 'true') {
      return process.env.REACT_APP_API_IP;
    }
    return window.location.host.split(':')[0];
  };
  
  const getAddress = () => {
    const protocol =
      process.env.REACT_APP_API_HTTPS === 'true' ? 'https://' : 'http://';
  
    return generateAddress(protocol);
  };
  
  const generateAddress = (protocol) => {
    const ip = getIp();
    debugger;
    const port = process.env.REACT_APP_API_PORT
  
    return `${protocol}${ip}${port}`;
  };
  
  export { getIp, getAddress};