'use client';

import { useEffect, useState } from 'react';
import { useParams, useRouter } from 'next/navigation';
import styles from '@/styles/recipts-page/hero/hero.module.css';
import { getResources } from '@/services/api/resourceClient';
import { getUnits } from '@/services/api/unitClient';
import { getReceiptById, updateReceipt, updateReceiptResources, deleteReceipt } from '@/services/api/receiptClient';

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
  const router = useRouter();
  const [receiptNumber, setReceiptNumber] = useState('');
  const [receiptDate, setReceiptDate] = useState('');
  const [resources, setResources] = useState<Resource[]>([]);
  const [units, setUnits] = useState<Unit[]>([]);
  const [receiptResources, setReceiptResources] = useState<ReceiptResource[]>([]);
  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);

  useEffect(() => {
    async function loadData() {
      try {
        setLoading(true);
        const [resourcesData, unitsData] = await Promise.all([
          getResources(),
          getUnits()
        ]);
        
        setResources(resourcesData);
        setUnits(unitsData);
        if (id) {
          const receiptData = await getReceiptById(id as string);
          setReceiptNumber(receiptData.number);
          setReceiptDate(receiptData.date.split('T')[0]);  //время из даты убираем
          const formattedResources = receiptData.receiptResources.map(rr => ({
            id: rr.id,
            resourceId: rr.resourceId,
            unitId: rr.unitId,
            quantity: rr.quantity
          }));
          setReceiptResources(formattedResources);
        }
      } catch (err) {
        console.error('Ошибка при загрузке данных:', err);
      } finally {
        setLoading(false);
      }
    }
    
    loadData();
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

  const handleSave = async () => {
    if (!id) return;
    
    setSaving(true);
    try {
      // Обновляем документ поступления
      await updateReceipt(id as string, {
        number: receiptNumber,
        date: receiptDate,
        resources: [] // Пустой массив, так как ресурсы обновляем отдельно
      });

      // Обновляем ресурсы поступления
      const validResources = receiptResources
        .filter(r => r.resourceId && r.unitId && r.quantity && r.quantity > 0)
        .map(r => ({
          resourceId: r.resourceId!,
          unitId: r.unitId!,
          quantity: r.quantity!
        }));

      await updateReceiptResources(id as string, validResources);

      alert('Поступление обновлено успешно!');
      router.push('/receipts-page');
    } catch (error) {
      console.error('Ошибка при сохранении:', error);
      alert(`Ошибка при сохранении: ${error instanceof Error ? error.message : 'Неизвестная ошибка'}`);
    } finally {
      setSaving(false);
    }
  };

  const handleDelete = async () => {
    if (!id || !confirm('Вы уверены, что хотите удалить это поступление?')) return;
    
    try {
      await deleteReceipt(id as string);
      alert('Поступление удалено успешно!');
      router.push('/receipts-page');
    } catch (error) {
      console.error('Ошибка при удалении:', error);
      alert(`Ошибка при удалении: ${error instanceof Error ? error.message : 'Неизвестная ошибка'}`);
    }
  };

  if (loading) {
    return <div>Загрузка...</div>;
  }

  return (
    <main className={styles.mainContainer}>
        <h1 className={styles.headerText}>Редактирование поступления</h1>
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
        <button className={styles.acceptButton} onClick={handleSave} disabled={saving}>
          {saving ? 'Сохраняем...' : 'Сохранить'}
        </button>
        <button className={styles.addButton} onClick={handleDelete}>Удалить поступление</button>
      </div>
    </main>
  );
}