import * as React from 'react';
import useSWR from 'swr';
import { Formik } from 'formik';
import { message, Alert, PageHeader, Divider, Button } from 'antd';
import { Input, Switch, InputNumber, SubmitButton, Form } from 'formik-antd';
import { useActions, badRequestResponseToFormikErrors } from './validation';

function toType(type: string, name: string) {
  switch (type) {
    case 'string':
      return <Input name={name} />;
    case 'number':
      return <InputNumber name={name} />;
    case 'boolean':
      return <Switch name={name} />;
    default:
      return <Input name={name} />;
  }
}

interface Props {
  title: string;
  path: string;
}

export async function getJson(key: string) {
  const r = await fetch(key);
  const data = await r.json();
  return data;
}

export function StronglyTypedOptions({ path, title }: Props) {
  const url = `/${path}`;
  const { submit } = useActions(url);
  const { data, error, revalidate } = useSWR(url, getJson);

  return (
    <div style={{ maxWidth: 1200, background: '#fff' }}>
      {error && <Alert type="error" message={error.toString()} />}
      <Formik
        initialValues={data}
        enableReinitialize={true}
        onSubmit={async (values, actions) => {
          console.log('submit');
          actions.setSubmitting(true);
          const r = await submit({ path, value: values });
          actions.setSubmitting(false);
          if (r.ok) {
            message.success('success');
          } else {
            if (r.status === 400) {
              const errors = await r.json();
              actions.setErrors(
                (badRequestResponseToFormikErrors(errors) as any).value
              );
            } else {
              message.error(r.statusText);
            }
          }
        }}>
        <Form>
          <PageHeader
            title={title}
            extra={[
              <SubmitButton size="small">save</SubmitButton>,
              // TODO somehow formik does not show the new values on revalidating
              // <Button
              //   size="small"
              //   key={2}
              //   icon="reload"
              //   onClick={() => revalidate()}>
              //   refresh
              // </Button>,
            ]}>
            <Divider />
            <div
              style={{
                display: 'grid',
                gridTemplateColumns: '160px auto',
              }}>
              {data &&
                Object.keys(data).map(v => (
                  <>
                    <label
                      style={{
                        marginTop: 10,
                        marginRight: 10,
                        textAlign: 'right',
                      }}>
                      {v}
                    </label>
                    <Form.Item name={v}>{toType(typeof data[v], v)}</Form.Item>
                  </>
                ))}
            </div>
          </PageHeader>
        </Form>
      </Formik>
    </div>
  );
}
