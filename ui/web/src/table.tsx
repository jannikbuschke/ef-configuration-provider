import * as React from 'react';
import { Formik } from 'formik';
import {
  Table,
  Form,
  AddRowButton,
  RemoveRowButton,
  Input,
  SubmitButton,
} from 'formik-antd';
import { Button, Alert, PageHeader, message, Tooltip } from 'antd';
import dayjs from 'dayjs';

function useGet<T>(url: string, placeholder: T) {
  const [key, setKey] = React.useState(Math.random());
  const [data, setData] = React.useState<T>(placeholder);
  const [error, setError] = React.useState('');
  const [loading, setLoading] = React.useState(false);
  const reload = React.useCallback(() => setKey(Math.random()), []);
  React.useEffect(() => {
    (async () => {
      try {
        setLoading(true);
        const response = await fetch(url);
        if (response.ok) {
          const data: T = await response.json();
          setError('');
          setData(data);
        } else {
          setError(response.statusText);
        }
      } catch (e) {
        setError(e.toString());
      } finally {
        setLoading(false);
      }
    })();
  }, [url, key]);
  return { data, error, loading, reload };
}

async function post<T>(url: string, payload: T) {
  return fetch(url, {
    method: 'POST',
    body: JSON.stringify(payload),
    headers: { 'content-type': 'application/json' },
  });
}

interface ConfigurationValue {
  name: string;
  value: string;
}

interface UpdateRequest {
  values: ConfigurationValue[];
}

interface Configuration {
  id: number;
  values: { [id: string]: string };
  created: string;
  user: string | null;
}

export function DataTable() {
  const [postError, setPostError] = React.useState('');
  const { data, loading, error, reload } = useGet<Configuration>(
    '/api/__configuration/current',
    { created: '', id: 0, user: null, values: {} }
  );
  const tableData = React.useMemo(() => {
    return Object.keys(data.values).map(
      v => ({ name: v, value: data.values[v] } as ConfigurationValue)
    );
  }, [data]);
  return (
    <div>
      <div>{error && <Alert type="error" message={error} />}</div>
      <div>{postError && <Alert type="error" message={postError} />}</div>
      <Formik<{ tableData: ConfigurationValue[] }>
        initialValues={{ tableData }}
        enableReinitialize={true}
        onSubmit={async (values, actions) => {
          const response = await post('/api/__configuration/update', {
            values: values.tableData,
          } as UpdateRequest);
          actions.setSubmitting(false);
          if (response.ok) {
            message.success('saved');
            setPostError('');
            reload();
          } else {
            message.error(response.statusText);
            const error = await response.text();
            setPostError(error);
          }
        }}>
        <Form>
          <PageHeader
            title="Configuration"
            subTitle={
              <Tooltip title="last edited">
                {dayjs(data.created).toString()}
              </Tooltip>
            }
            extra={[
              <AddRowButton
                key={1}
                name="tableData"
                style={{ marginBottom: 20 }}
                createNewRow={() => ({
                  name: 'id',
                  value: 'value',
                })}>
                Add
              </AddRowButton>,
              <Button key={2} icon="reload" onClick={() => reload()}>
                refresh
              </Button>,
              <SubmitButton key={3}>save</SubmitButton>,
            ]}></PageHeader>

          <Table
            loading={loading}
            name="tableData"
            rowKey={(row, index) => '' + index}
            style={{}}
            size="small"
            pagination={false}
            columns={[
              {
                width: 300,
                title: 'Name',
                key: 'name',
                render: (text, record, i) => (
                  <Input
                    style={{ border: 'none' }}
                    name={`tableData.${i}.name`}
                  />
                ),
              },
              {
                title: 'Value',
                key: 'value',
                render: (text, record, i) => (
                  <Input
                    style={{ border: 'none' }}
                    name={`tableData.${i}.value`}
                  />
                ),
              },
              {
                width: 32,
                key: 'actions',
                align: 'right',
                render: (text, record, index) => (
                  <RemoveRowButton
                    style={{ border: 'none' }}
                    icon="delete"
                    name="tableData"
                    index={index}
                  />
                ),
              },
            ]}
          />
        </Form>
      </Formik>
    </div>
  );
}
