import * as React from 'react';
import { set } from 'lodash';
import { FormikErrors } from 'formik';

interface BadRequestResponse {
  [field: string]: string[];
}

function camelize(str: string) {
  return str
    .split('.')
    .map(_camelize)
    .join('.');
}

function _camelize(str: string) {
  return str.replace(/(?:^\w|[A-Z]|\b\w|\s+)/g, function(match, index) {
    if (+match === 0) return ''; // or if (/\s+/.test(match)) for white spaces
    return index == 0 ? match.toLowerCase() : match.toUpperCase();
  });
}

export function badRequestResponseToFormikErrors<T = any>(
  data: BadRequestResponse
) {
  const errors: FormikErrors<T> = {};
  Object.keys(data).forEach(key => {
    const path = camelize(key);
    set(errors, path, data[key]);
  });
  console.log({ errors });
  return errors;
}

export function useActions(url: string) {
  const [error, setError] = React.useState('');

  function send(values: any, intent: 'execute' | 'validate') {
    return fetch(url, {
      method: 'POST',
      body: JSON.stringify(values),
      headers: {
        'x-action-intent': intent,
        'content-type': 'application/json',
      },
    });
  }

  return {
    submit: (values: any) => send(values, 'execute'),
    validate: async (values: any): Promise<any> => {
      const response = await send(values, 'validate');

      switch (response.status) {
        case 400: {
          const data: BadRequestResponse = await response.json();
          const errors = badRequestResponseToFormikErrors(data);

          console.log('errors', errors);
          if (Object.keys(errors).length) {
            return errors;
          }
        }
        case 204: {
          return {};
        }
        case 404: {
          throw Error(
            `could not find validation handler '${url}'. Did you forget to provide a server side validation handler?`
          );
        }
        default: {
          setError(`${response.statusText} ${response.status}`);
          console.error(response);
        }
      }
    },
  };
}
