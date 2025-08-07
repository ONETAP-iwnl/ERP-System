'use client';

const BASE_URL = process.env.NEXT_PUBLIC_API_URL || 'http://localhost:7015/api'; 

export async function createUnit(name: string) {
  const response = await fetch(`${BASE_URL}/units/create`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({ name, isActive: true }),
  });

  if (!response.ok) {
    throw new Error('Ошибка при создании единицы измерения');
  }

  return await response.json();
}

export async function getInactiveUnits() {
  const response = await fetch(`${BASE_URL}/units/inactive`);

  if (!response.ok) {
    const message = await response.text();
    throw new Error(`Ошибка при получении архивных единиц: ${message}`);
  }

  return await response.json();
}

export async function getUnitById(id: number) {
  const response = await fetch(`${BASE_URL}/units/${id}`);
  if (!response.ok) {
    throw new Error('Ошибка при получении единицы измерения');
  }
  return await response.json();
}

export async function updateUnit(id: number, name: string, isActive: boolean = true) {
  const response = await fetch(`${BASE_URL}/units/${id}`, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ id, name, isActive }),
  });
    if (!response.ok) {
    const errorText = await response.text();
    throw new Error(`Ошибка при обновлении единицы измерения: ${errorText}`);
  }
  return await response.json();
}

export async function archiveUnit(id: number) {
  const response = await fetch(`${BASE_URL}/units/${id}/archive`, {
    method: 'PUT',
  });
  if (!response.ok) {
    throw new Error('Ошибка при архивировании единицы измерения');
  }
  return await response.json();
}

export async function deleteUnit(id: number) {
  const response = await fetch(`${BASE_URL}/units/${id}`, {
    method: 'DELETE',
  });
  if (!response.ok) {
    throw new Error('Ошибка при удалении единицы измерения');
  }
}