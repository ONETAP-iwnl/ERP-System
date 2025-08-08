'use client';

import { useState, useEffect } from 'react';
import styles from '@/styles/recipts-page/hero/hero.module.css'
import { getUnits } from '@/services/api/unitClient';
import { getResources } from '@/services/api/resourceClient';
import { ReceiptResourceDto } from '@/services/api/receiptClient';

const BASE_URL = process.env.NEXT_PUBLIC_API_URL || 'http://localhost:7015/api';

export default function Hero() {
  const [receiptNumber, setReceiptNumber] = useState('');
  const [receiptDate, setReceiptDate] = useState(() => new Date().toISOString().split('T')[0]);
  const [resources, setResources] = useState<{ id: number; name: string }[]>([]);
  const [units, setUnits] = useState<{ id: number; name: string }[]>([]);
  const [receiptResources, setReceiptResources] = useState<ReceiptResourceDto[]>([]);
  const [isSaving, setIsSaving] = useState(false);

  useEffect(() => {
    Promise.all([getResources(), getUnits()])
      .then(([resData, unitData]) => {
        setResources(resData);
        setUnits(unitData);
      })
      .catch((err) => console.error('Ошибка загрузки данных:', err));
  }, []);

  const handleAddRow = () => {
    const newRow: ReceiptResourceDto = {
        resourceId: 0,
        unitId: 0,
        quantity: 0,
        id: 0,
        resourceName: '',
        unitName: '',
        receiptDocumentId: 0
    };
    setReceiptResources((prev) => [...prev, newRow]);
  };

  const handleDeleteRow = (index: number) => {
    setReceiptResources((prev) => prev.filter((_, i) => i !== index));
  };

  const updateRow = (index: number, key: keyof ReceiptResourceDto, value: any) => {
    setReceiptResources((prev) =>
      prev.map((row, i) => (i === index ? { ...row, [key]: value } : row))
    );
  };

    const handleSave = async () => {
        setIsSaving(true);
        try {
            // 1. Создаём документ поступления
            const receiptRes = await fetch(`${BASE_URL}/ReceiptDocuments`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({
            number: receiptNumber,
            date: receiptDate
      }),
    });

    if (!receiptRes.ok) {
      throw new Error(await receiptRes.text() || 'Ошибка создания поступления');
    }

    const receipt = await receiptRes.json();
    const receiptId = receipt.id;

    // 2. Фильтруем ресурсы, которые имеют все необходимые данные
    const validResources = receiptResources.filter(r => r.resourceId && r.unitId && r.quantity > 0);

    // 3. Отправляем каждый ресурс отдельным запросом
    for (const resource of validResources) {
      const resRes = await fetch(`${BASE_URL}/ReceiptResources`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
          resourceId: resource.resourceId,
          unitId: resource.unitId,
          quantity: resource.quantity,
          receiptDocumentId: receiptId
        }),
      });

      if (!resRes.ok) {
        throw new Error(await resRes.text() || 'Ошибка добавления ресурса');
      }
    }

    alert('Поступление создано успешно!');

  } catch (error) {
    console.error(error);
    alert(`Ошибка: ${error instanceof Error ? error.message : 'Неизвестная ошибка'}`);
  } finally {
    setIsSaving(false);
  }
};

  return (
    <main className={styles.mainContainer}>
      <h1 className={styles.headerText}>Создание поступления</h1>

      <div className={styles.filterBox}>
        <div className={styles.inputGroup}>
          <label>Номер поступления</label>
          <input
            className={styles.inputNumber}
            type="text"
            value={receiptNumber}
            onChange={(e) => setReceiptNumber(e.target.value)}
            placeholder="Введите номер документа"
          />
        </div>
      </div>

      <div className={styles.inputGroup}>
        <label className={styles.headerText}>Дата</label>
        <input
          className={styles.dateStart}
          type="date"
          value={receiptDate}
          onChange={(e) => setReceiptDate(e.target.value)}
        />
      </div>

      <table className={styles.table}>
        <thead>
          <tr>
            <th>
              <button
                onClick={handleAddRow}
                className={styles.addButton}
                title="Добавить ресурс"
              >
                +
              </button>
            </th>
            <th>Ресурс</th>
            <th>Ед. изм.</th>
            <th>Кол-во</th>
          </tr>
        </thead>
        <tbody>
          {receiptResources.map((row, index) => (
            <tr key={index}>
              <td>
                <button
                  onClick={() => handleDeleteRow(index)}
                  className={styles.deleteButton}
                  title="Удалить строку"
                >
                  −
                </button>
              </td>
              <td>
                <select
                  className={styles.select}
                  value={row.resourceId || ''}
                  onChange={(e) => updateRow(index, 'resourceId', parseInt(e.target.value))}
                >
                  <option value="">Выберите ресурс</option>
                  {resources.map((res) => (
                    <option key={res.id} value={res.id}>
                      {res.name}
                    </option>
                  ))}
                </select>
              </td>
              <td>
                <select
                  className={styles.select}
                  value={row.unitId || ''}
                  onChange={(e) => updateRow(index, 'unitId', parseInt(e.target.value))}
                >
                  <option value="">Выберите ед. изм.</option>
                  {units.map((unit) => (
                    <option key={unit.id} value={unit.id}>
                      {unit.name}
                    </option>
                  ))}
                </select>
              </td>
              <td>
                <input
                  className={styles.dateStart}
                  type="number"
                  min="0"
                  value={row.quantity || ''}
                  onChange={(e) => updateRow(index, 'quantity', parseFloat(e.target.value))}
                  placeholder="Количество"
                />
              </td>
            </tr>
          ))}
        </tbody>
      </table>

      <div className={styles.buttonGroup}>
        <button
          className={styles.acceptButton}
          onClick={handleSave}
          disabled={isSaving}
        >
          {isSaving ? 'Сохраняем...' : 'Сохранить'}
        </button>
      </div>
    </main>
  );
}
