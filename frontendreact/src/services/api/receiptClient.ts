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

export async function getReceiptById(id: string): Promise<ReceiptDto> {
  const res = await fetch(`${BASE_URL}/receipt/${id}`, { cache: 'no-store' });
  if (!res.ok) throw new Error('Ошибка получения поступления');
  return res.json();
}

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

export async function deleteReceipt(id: string) {
  const res = await fetch(`${BASE_URL}/receipt/${id}`, {
    method: 'DELETE',
  });
  if (!res.ok) throw new Error('Ошибка удаления поступления');
}

