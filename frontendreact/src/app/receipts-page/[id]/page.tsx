'use client';

import { useEffect, useState } from 'react';
import { useParams } from 'next/navigation';
import styles from '@/styles/recipts-page/hero/hero.module.css';

type Resource = {
  id: number;
  name: string;
};

type Unit = {
  id: number;
  name: string;
};

type ReceiptResource = {
  id: number;
  resourceId: number | null;
  unitId: number | null;
  quantity: number | null;
};

export default function EditReceipt() {
  const { id } = useParams();
  const [receiptNumbers, setReceiptNumber] = useState('asa');
  const [receiptDate, setReceiptDate] = useState('2025-07-30');
  const [resources, setResources] = useState<Resource[]>([]);
  const [units, setUnits] = useState<Unit[]>([]);
  const [receiptResources, setReceiptResources] = useState<ReceiptResource[]>([]);

  useEffect(() => {
    // Заглушка ресурсов и единиц измерения
    setResources([
      { id: 1, name: 'Ноутбук' },
      { id: 2, name: 'Клавиатура' },
      { id: 3, name: 'Монитор' },
    ]);

    setUnits([
      { id: 1, name: 'шт' },
      { id: 2, name: 'кг' },
    ]);

    setReceiptResources([
      { id: 101, resourceId: 1, unitId: 1, quantity: 5 },
    ]);
  }, [id]);

  const handleAddRow = () => {
    const newRow: ReceiptResource = {
      id: Date.now(),
      resourceId: null,
      unitId: null,
      quantity: null,
    };
    setReceiptResources((prev) => [...prev, newRow]);
  };

  const handleDeleteRow = (id: number) => {
    setReceiptResources((prev) => prev.filter((row) => row.id !== id));
  };

  const updateRow = (id: number, key: keyof ReceiptResource, value: any) => {
    setReceiptResources((prev) =>
      prev.map((row) =>
        row.id === id ? { ...row, [key]: value } : row
      )
    );
  };

  const handleSave = () => {
    console.log('Сохраняем:', {
      id,
      number: receiptNumbers,
      receiptDate,
      resources: receiptResources,
    });
  };

  return (
    <main className={styles.mainContainer}>
        <h1 className={styles.headerText}>Редактирование поступления</h1>
        <div className={styles.filterBox}>
            <div className={styles.inputGroup}>
            <label>Номер поступления</label>
            <input
                className={styles.inputNumber}
                type="text"
                value={receiptNumbers}
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
          {receiptResources.map((row) => (
            <tr key={row.id}>
              <td>
                <button
                  onClick={() => handleDeleteRow(row.id)}
                  className={styles.deleteButton}
                  title="Удалить строку"
                >
                  −
                </button>
              </td>
              <td>
                <select
                  className={styles.select}
                  value={row.resourceId ?? ''}
                  onChange={(e) =>
                    updateRow(row.id, 'resourceId', parseInt(e.target.value))
                  }
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
                  value={row.unitId ?? ''}
                  onChange={(e) =>
                    updateRow(row.id, 'unitId', parseInt(e.target.value))
                  }
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
                  value={row.quantity ?? ''}
                  onChange={(e) =>
                    updateRow(row.id, 'quantity', parseFloat(e.target.value))
                  }
                  placeholder="Количество"
                />
              </td>
            </tr>
          ))}
        </tbody>
      </table>

      <div className={styles.buttonGroup}>
        <button className={styles.acceptButton} onClick={handleSave}>
          Сохранить
        </button>
        <button className={styles.addButton}>Удалить поступление</button>
      </div>
    </main>
  );
}