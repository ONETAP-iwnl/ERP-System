'use client';

const BASE_URL = process.env.NEXT_PUBLIC_API_URL || 'http://localhost:7015/api'; 

export interface ReceiptResourceDto {
  id: number;
  resourceId: number;
  resourceName: string;
  unitId: number;
  unitName: string;
  quantity: number;
  receiptDocumentId: number;
}

export interface ReceiptDto {
  id: number;
  number: string;
  date: string;
  receiptResources: ReceiptResourceDto[];
}

export interface UpdateReceiptDto {
  number: string;
  date: string;
  resources: {
    resourceId: number;
    unitId: number;
    quantity: number;
  }[];
}

export interface ReceiptFilter {
  startDate?: string;
  endDate?: string;
  documentNumbers?: string[];
  resourceIds?: number[];
  unitIds?: number[];
}

export async function getReceipts(): Promise<ReceiptDto[]> {
    const res = await fetch(`${BASE_URL}/receiptdocuments`, { cache: 'no-store' });
    if (!res.ok) throw new Error('Ошибка загрузки поступлений');
    return res.json();
}

export async function getFilteredReceipts(filter: ReceiptFilter): Promise<ReceiptDto[]> {
    const params = new URLSearchParams();
    
    if (filter.startDate) params.append('startDate', filter.startDate);
    if (filter.endDate) params.append('endDate', filter.endDate);
    if (filter.documentNumbers && filter.documentNumbers.length > 0) {
        filter.documentNumbers.forEach(num => params.append('documentNumbers', num));
    }
    if (filter.resourceIds && filter.resourceIds.length > 0) {
        filter.resourceIds.forEach(id => params.append('resourceIds', id.toString()));
    }
    if (filter.unitIds && filter.unitIds.length > 0) {
        filter.unitIds.forEach(id => params.append('unitIds', id.toString()));
    }
    
    const res = await fetch(`${BASE_URL}/receiptdocuments/filter?${params.toString()}`, { cache: 'no-store' });
    if (!res.ok) throw new Error('Ошибка фильтрации поступлений');
    return res.json();
}

export async function getReceiptById(id: string): Promise<ReceiptDto> {
  const res = await fetch(`${BASE_URL}/receiptdocuments/${id}`, { cache: 'no-store' });
  if (!res.ok) throw new Error('Ошибка получения поступления');
  return res.json();
}

export async function updateReceipt(id: string, data: UpdateReceiptDto) {
  const res = await fetch(`${BASE_URL}/receiptdocuments/${id}`, {
    method: 'PUT',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({
      id: parseInt(id, 10),
      number: data.number,
      date: data.date
    }),
  });
  if (!res.ok) throw new Error('Ошибка обновления поступления');
}

export async function updateReceiptResources(documentId: string, resources: { resourceId: number; unitId: number; quantity: number }[]) {
  // Сначала удаляем все существующие ресурсы
  const existingResources = await getReceiptById(documentId);
  for (const resource of existingResources.receiptResources) {
    const deleteRes = await fetch(`${BASE_URL}/receiptresources/${resource.id}`, {
      method: 'DELETE',
    });
    if (!deleteRes.ok) {
      console.warn(`Не удалось удалить ресурс ${resource.id}:`, await deleteRes.text());
    }
  }

  // Затем добавляем новые ресурсы
  for (const resource of resources) {
    const res = await fetch(`${BASE_URL}/receiptresources`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({
        resourceId: resource.resourceId,
        unitId: resource.unitId,
        quantity: resource.quantity,
        receiptDocumentId: parseInt(documentId, 10)
      }),
    });
    if (!res.ok) throw new Error('Ошибка обновления ресурсов поступления');
  }
}

export async function deleteReceipt(id: string) {
  const res = await fetch(`${BASE_URL}/receiptdocuments/${id}`, {
    method: 'DELETE',
  });
  if (!res.ok) throw new Error('Ошибка удаления поступления');
}

