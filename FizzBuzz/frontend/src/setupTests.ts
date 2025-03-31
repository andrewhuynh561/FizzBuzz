import '@testing-library/jest-dom';

Object.assign(global, {
  TextDecoder: require('util').TextDecoder,
  TextEncoder: require('util').TextEncoder,
}); 