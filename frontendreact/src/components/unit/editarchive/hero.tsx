'use client';

import { useRouter, useParams } from 'next/navigation';
import { useEffect, useState } from 'react';
import styles from "@/styles/recipts-page/hero/hero.module.css";
import { getUnitById, updateUnit, archiveUnit, deleteUnit } from '@/services/api/unitClient';

export default function EditArchiveUnit() {
  const router = useRouter();
  const params = useParams();
  const id = Number(params.id);

  const [inputName, setInputName] = useState('');
  const [isActive, setIsActive] = useState(false); // архивная - неактивная
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    async function fetchUnit() {
      try {
        const unit = await getUnitById(id);
        setInputName(unit.name);
        setIsActive(unit.isActive);
      } catch (e) {
        setError('Ошибка при загрузке единицы измерения');
      }
    }
    if (id) {
      fetchUnit();
    }
  }, [id]);

  async function handleSave() {
    try {
      await updateUnit(id, inputName, isActive);
      router.push('/unit/archive'); // или куда нужно после сохранения
    } catch (e) {
      setError('Ошибка при сохранении единицы измерения');
    }
  }

  async function handleDelete() {
    try {
      await deleteUnit(id);
      router.push('/unit/archive');
    } catch (e) {
      setError('Ошибка при удалении единицы измерения');
    }
  }

  async function handleActivate() {
    try {
      await updateUnit(id, inputName, true);
      router.push('/unit-page');
    } catch (e) {
      setError('Ошибка при переводе в активные');
    }
  }

  return (
    <main className={styles.mainContainer}>
      <h1 className={styles.headerText}>Единица измерения (архив)</h1>

      {error && <p style={{color:'red'}}>{error}</p>}

      <div className={styles.buttonGroup}>
        <button className={styles.addButton} onClick={handleSave}>Сохранить</button>
        <button className={styles.deleteButton} onClick={handleDelete}>Удалить</button>
        <button className={styles.acceptButton} onClick={handleActivate}>В работу</button>
      </div>

      <div className={styles.inputGroup}>
        <h4 className={styles.filterText}>Наименование</h4>
        <input
          type='text'
          className={styles.inputNumber}
          value={inputName}
          onChange={(e) => setInputName(e.target.value)}
        />
      </div>
    </main>
  );
}
