'use client';

const BASE_URL = process.env.NEXT_PUBLIC_API_URL || 'http://localhost:7015/api'; 

export interface ReceiptResourceDto {
  resourceId: number;
  unitId: number;
  quantity: number;
}

export interface ReceiptDto {
  id: number;
  number: string;
  date: string;
  resources: {
    id: number;
    name: string;
    unit: string;
    quantity: number;
  }[];
}

export interface UpdateReceiptDto {
  number: string;
  date: string;
  resources: ReceiptResourceDto[];
}

// Получить поступление по ID
export async function getReceiptById(id: string): Promise<ReceiptDto> {
  const res = await fetch(`${BASE_URL}/receipt/${id}`, { cache: 'no-store' });
  if (!res.ok) throw new Error('Ошибка получения поступления');
  return res.json();
}

// Обновить поступление
export async function updateReceipt(id: string, data: UpdateReceiptDto) {
  const res = await fetch(`${BASE_URL}/receipt/${id}`, {
    method: 'PUT',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(data),
  });
  if (!res.ok) throw new Error('Ошибка обновления поступления');
}

// Удалить поступление
export async function deleteReceipt(id: string) {
  const res = await fetch(`${BASE_URL}/receipt/${id}`, {
    method: 'DELETE',
  });
  if (!res.ok) throw new Error('Ошибка удаления поступления');
}

// Получить список ресурсов
export async function getResources(): Promise<{ id: number; name: string }[]> {
  const res = await fetch(`${BASE_URL}/resources`, { cache: 'no-store' });
  if (!res.ok) throw new Error('Ошибка загрузки ресурсов');
  return res.json();
}

// Получить список единиц измерения
export async function getUnits(): Promise<{ id: number; name: string }[]> {
  const res = await fetch(`${BASE_URL}/Units`, { cache: 'no-store' });
  if (!res.ok) throw new Error('Ошибка загрузки единиц измерения');
  return res.json();
}