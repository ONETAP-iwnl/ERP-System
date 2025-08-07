'use client';

const BASE_URL = process.env.NEXT_PUBLIC_API_URL || 'http://localhost:7015/api'; 

export async function createResource(name: string) {
  const response = await fetch(`${BASE_URL}/resources`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({ name, isActive: true }),
  });

  if (!response.ok) {
    throw new Error('Ошибка при создании ресурса');
  }

  return await response.json();
}

export async function getInactiveResources() {
  const response = await fetch(`${BASE_URL}/resources/inactive`);

  if (!response.ok) {
    const message = await response.text();
    throw new Error(`Ошибка при получении архивных ресурсов: ${message}`);
  }

  return await response.json();
}

export async function getResourceById(id: number) {
  const response = await fetch(`${BASE_URL}/resources/${id}`);
  if (!response.ok) {
    throw new Error('Ошибка при получении ресурса');
  }
  return await response.json();
}

export async function updateResource(id: number, name: string, isActive: boolean = true) {
  const response = await fetch(`${BASE_URL}/resources/${id}`, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ id, name, isActive }),
  });
  if (!response.ok) {
    const errorText = await response.text();
    throw new Error(`Ошибка при обновлении ресурса: ${errorText}`);
  }
  return await response.json();
}

export async function archiveResource(id: number) {
  const response = await fetch(`${BASE_URL}/resources/${id}/archive`, {
    method: 'PUT',
  });
  if (!response.ok) {
    throw new Error('Ошибка при архивировании ресурса');
  }
  return await response.json();
}

export async function deleteResource(id: number) {
  const response = await fetch(`${BASE_URL}/resources/${id}`, {
    method: 'DELETE',
  });
  if (!response.ok) {
    throw new Error('Ошибка при удалении ресурса');
  }
}

export async function getResources(): Promise<{ id: number; name: string; isActive: boolean }[]> {
  const res = await fetch(`${BASE_URL}/Resources`, { cache: 'no-store' });
  if (!res.ok) throw new Error('Ошибка загрузки ресурсов');
  return res.json();
}